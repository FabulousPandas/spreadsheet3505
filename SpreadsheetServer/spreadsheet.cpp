/*
 Spreadsheet class. This spreadsheet object can holds the cells 
 of the spreadsheet and the connects (clients)
 Written by Malik Qader 4/25/2021

*/


#include "spreadsheet.h"
#include <boost/filesystem.hpp>
#include <iostream> //TODO REMOVE
#include "handle_connection.h"

spreadsheet::spreadsheet()
{
	spreadsheet_name = "";
}

/*
* Spreadsheet constructor
*/
spreadsheet::spreadsheet(std::string filepath)
{
	spreadsheet_name = filepath;
	build_from_file();
}

/*
* adds a message from the client to the mesasge_q
*/
void spreadsheet::add_to_q(std::vector<std::string> message)
{
	message_q.push(message);
}

void spreadsheet::build_from_file()
{
	boost::filesystem::path sheet_dir(spreadsheet_name);
	boost::filesystem::ifstream file(sheet_dir);
	std::string str;
	int history_index = 0;
	int message_index = 0;
	std::vector<std::string> message;
	message.push_back("");
	message.push_back("");
	message.push_back("");
	while (std::getline(file, str))
	{
		if(str == "")
		{
			change_history.push_back(message);
			history_index++;
			message_index = 0;
		}
		else
		{
			message.at(message_index) = str;
			message_index++;
		}
	}
	file.close();
}

/*
* Saves the spreadsheet
*/
void spreadsheet::remake_file_from_history()
{
	boost::filesystem::path sheet_dir(spreadsheet_name);
	boost::filesystem::ofstream file(sheet_dir);
	for (int i = 0; i < change_history.size(); i++)
	{
		std::vector<std::string> message = change_history.at(i);
		for (int j = 0; j < message.size(); j++)
		{
			file << message.at(j) << '\n';
		}
		file << '\n';
	}
	file.close();
}

/*
* Implements one message from the message_q
* return "pass" if it was succesful 
* return "empty" if the queue is empty
* otherwise will return a string describing
* the error
*/
std::string spreadsheet::proccess_next_message()
{
	if (message_q.size() == 0)
		return "empty";

	std::vector<std::string> message = message_q.front();
	message_q.pop();

	if (!is_dependent(message))
	{
		if (message.at(0) == "editCell")
		{
			if (message.size() != 3)
				return "Invalid number of data values sent in editCell JSON";
			cell* this_cell = get_cell(message.at(1));
			this_cell->add_edit(message.at(2));
			change_history.push_back(message);
			remake_file_from_history();
			
			message.at(0) = "cellUpdated";
			for (int i = 0; i < client_list.size(); i++)
			{
				handle_connection* client = client_list.at(i);
				client->server_response(message);
			}
		}
		/*else if (message.at(0) == "revertCell")
		{
			if (message.size() != 2)
				return "Invalid number of data values sent in revertCell JSON";

			cell this_cell = get_cell(message.at(1));	
			this_cell.revert();
			change_history.push_back(message);
			write_message_to_spreadsheet(message);	
		}*/
		else if (message.at(0) == "selectCell")
		{
			if(message.size() != 4)
				return "Invalid number of data values sent in selectCell JSON";
			
			message.at(0) = "cellSelected";
			for (int i = 0; i < client_list.size(); i++)
			{
				handle_connection* client = client_list.at(i);
				client->server_response(message);
			}

		}
		else if (message.at(0) == "undo")
		{
			if (message.size() != 1)
				return "Invalid number of data values sent in undo JSON";

			if (change_history.size() == 0)
				return "Tried to undo a spreadsheet with no changes";

			std::vector<std::string> prev_message = change_history.back();
			cell* prev_cell = get_cell(prev_message.at(1));
			std::cout << "TO GO BACK TO " << prev_message.at(0) << ": " << prev_message.at(1) << std::endl;
			if (prev_message.at(0) == "editCell")
			{
				prev_cell->remove_edit();
				std::cout << "HEY MALIK" << std::endl;
				message.at(0) = "cellUpdated";
				message.push_back(prev_message.at(1));
				message.push_back(prev_cell->cell_content());
				for (int i = 0; i < client_list.size(); i++)
				{
					handle_connection* client = client_list.at(i);
					client->server_response(message);
				}

			} 
			/*else if (prev_message.at(0) == "revertCell")
			{
				prev_cell.undo_revert();
			}*/
			change_history.pop_back();
			remake_file_from_history();
		}
		else
		{
			return "Unknown requestType sent to server";
		}
	}


	return "pass";
}
	
bool spreadsheet::needs_to_proccess_message()
{
	return message_q.size() != 0;
}

cell* spreadsheet::get_cell(std::string cell_name)
{

	std::map<std::string, cell*>::iterator it = cell_map.find(cell_name);
	if (it == cell_map.end()) // If the cell map doesn't contain this cell
	{
		it = cell_map.insert(std::pair<std::string, cell*>(cell_name, new cell(cell_name))).first; // Add a new cell to the map
	}
 	return it->second; // Gets the cell value from the cell name key specified earlier
}

/*
* Adds a client useing their id to client_list
*/
void spreadsheet::add_client(handle_connection* client)
{
	client_list.push_back(client);
	for (int i = 0; i < change_history.size(); i++)
	{
		if (i == (change_history.size() - 1))
			client->con_state = 2;

		std::vector<std::string> message = change_history.at(i);
		message.at(0) = "cellUpdated";
		client->server_response(message);
	}
}

/*
* removes client from list, 
* changes client_list's index to -1 if found
*/
void spreadsheet::remove_client(handle_connection* client)
{
	for (int i = 0; i < client_list.size(); i++)
	{
		//if id is found then change to -1
		//if (client == client_list[i])
			//client_list[i] = -1;
	}
}

/*
* returns the list of clients currently connected
* to this spreadsheet
*/
std::vector<handle_connection*> spreadsheet::give_client()
{
	std::vector<handle_connection*> active_clients;
	

	for (int i = 0; i < client_list.size(); i++)
	{
		//if (client_list[i] != -1)
		//{
			//active_clients.push_back(client_list[i]);
		//}
	}

	return active_clients;
}

/*
* returns false if the message would not cause a
* dependency in the spreadsheet
*/
bool spreadsheet::is_dependent(std::vector<std::string> message)
{

	//TODO

	return false;
}
