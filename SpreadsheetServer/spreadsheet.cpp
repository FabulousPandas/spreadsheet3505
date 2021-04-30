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
	int history_index = 1;
	int message_index = 0;
	std::vector<std::string> message;
	message.push_back("");
	message.push_back("");
	message.push_back("");
	cell* this_cell;
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
			if (message_index == 1)
			{
				this_cell = get_cell(str);
			}
			if (message_index == 2)
			{
				this_cell->add_edit(str);
			}
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
*/
void spreadsheet::proccess_next_message()
{
	std::vector<std::string> message = message_q.front();
	message_q.pop();

	if (!is_cyclic_dependency())
	{
		if (message.at(0) == "editCell")
		{
			cell* this_cell = get_cell(message.at(1));
			this_cell->add_edit(message.at(2));
			if(is_cyclic_dependency())
			{
				send_client_error(message.at(3), message.at(1), "Edit would cause cyclic dependency");
				this_cell->remove_edit();
				return;
			}
			message.pop_back();
			message.resize(3);
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
			message.at(0) = "cellSelected";
			for (int i = 0; i < client_list.size(); i++)
			{
				handle_connection* client = client_list.at(i);
				client->server_response(message);
			}

		}
		else if (message.at(0) == "undo")
		{
			if (change_history.size() == 0)
			{
				send_client_error(message.at(1), "", "Tried to undo a spreadsheet with no changes");
				return;
			}

			std::vector<std::string> prev_message = change_history.back();
			cell* prev_cell = get_cell(prev_message.at(1));
			if (prev_message.at(0) == "editCell")
			{
				for (int i = 0; i < prev_message.size(); i++)
				{
					std::cout << "GO BACK TO THIS: " << prev_message.at(i) << std::endl;
				}
				prev_cell->remove_edit();
				message.at(0) = "cellUpdated";
				message.at(1) = prev_message.at(1);
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
	}
}
	
void spreadsheet::send_client_error(std::string client_id, std::string cell_name, std::string error_message)
{
	handle_connection* client = id_to_client[std::stoi(client_id)];
	std::vector<std::string> message;
	message.push_back("requestError");
	message.push_back(cell_name);
	message.push_back(error_message);
	client->server_response(message);
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
* Adds a client using their id to client_list
*/
void spreadsheet::add_client(handle_connection* client)
{
	client_list.push_back(client);
	
	if(change_history.size() == 0)
	{
		client->con_state = 2;
		std::vector<std::string> message;
		message.push_back("cellUpdated");
		message.push_back("A1");
		message.push_back("");
		client->server_response(message);
	}

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
 * Adds an id-client object map pairing to the spreadsheet
 */ 
void spreadsheet::add_client_id(int id, handle_connection* client)
{
	id_to_client.insert(std::pair<int, handle_connection*>(id, client));
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

void spreadsheet::disconnect_client(int id, handle_connection* client)
{
	id_to_client.erase(id);
	client_list.erase(std::find(client_list.begin(), client_list.end(), client));
	std::vector<std::string> message;
	message.push_back("disconnected");
	message.push_back(std::to_string(id));
	for(int i = 0; i < client_list.size(); i++)
	{
		handle_connection* client = client_list.at(i);
		client->server_response(message);
	}
}

/*
 * Called when the server shuts down
 */
void spreadsheet::server_shutdown(std::string shutdown_msg)
{
	std::vector<std::string> message;
	message.push_back("serverError");
	message.push_back(shutdown_msg);
	for(int i = 0; i < client_list.size(); i++)
	{
		handle_connection* client = client_list.at(i);
		client->server_response(message);
	}
}

/*
* returns false if there is not a
* cyclic dependency in the spreadsheet
*/
bool spreadsheet::is_cyclic_dependency()
{
	/*std::vector<std::string> visited;
	for (std::map<std::string, cell*>::iterator it; it != cell_map.end(); it++)
	{
		std::string cur_cellname = it->first;
		cell* cur_cell = it->second;
		if(visited.find(visited.begin(), visited.end(), cur_cellname) != visited.end()) // if the 
		{
		}
	}*/
	return false;
}
