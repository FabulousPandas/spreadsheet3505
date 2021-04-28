/*
 Spreadsheet class. This spreadsheet object can holds the cells 
 of the spreadsheet and the connects (clients)
 Written by Malik Qader 4/25/2021

*/


#include "spreadsheet.h"
#include <boost/filesystem.hpp>
#include <iostream> //TODO REMOVE

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
	file.close();
}

/*
* Saves the spreadsheet
*/
void spreadsheet::write_message_to_spreadsheet(std::vector<std::string> message)
{
	boost::filesystem::path sheet_dir(spreadsheet_name);
	boost::filesystem::ofstream file(sheet_dir);
	for (int i = 0; i < message.size(); i++)
	{
		file << message.at(i) << '\n';
	}
	file << '\n';
	file.close();
}

void spreadsheet::remake_file_from_history()
{

}

/*
* Implements one message from the message_q
* return "pass" if it was succesful 
* otherwise will return a string describing
* the error
*/
std::string spreadsheet::proccess_next_message()
{
	if (message_q.size() == 0)
		return "Tried to read from an empty queue";

	std::vector<std::string> message = message_q.front();
	message_q.pop();

	if (!is_dependent(message))
	{
		if (message.at(0) == "editCell")
		{
			if (message.size() != 3)
				return "Invalid number of data values sent in editCell JSON";
			cell this_cell = get_cell(message.at(1));
			this_cell.add_edit(message.at(2));
			change_history.push_back(message);
			write_message_to_spreadsheet(message);
		}
		else if (message.at(0) == "revertCell")
		{
			if (message.size() != 2)
				return "Invalid number of data values sent in revertCell JSON";

			cell this_cell = get_cell(message.at(1));	
			this_cell.revert();
			change_history.push_back(message);
			write_message_to_spreadsheet(message);	
		}
		else if (message.at(0) == "selectCell")
		{
		}
		else if (message.at(0) == "undo")
		{
			if (message.size() != 1)
				return "Invalid number of data values sent in undo JSON";

			std::vector<std::string> prev_message = change_history.back();
			cell prev_cell = get_cell(prev_message.at(1));
			if (prev_message.at(0) == "editCell")
			{
				prev_cell.remove_edit();
			} 
			else if (prev_message.at(0) == "revertCell")
			{
				prev_cell.undo_revert();
			}
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

cell spreadsheet::get_cell(std::string cell_name)
{

	std::map<std::string, cell>::iterator it = cell_map.find(cell_name);
	if (it == cell_map.end()) // If the cell map doesn't contain this cell
	{
		it = cell_map.insert(std::pair<std::string, cell>(cell_name, cell(cell_name))).first; // Add a new cell to the map
	}
 	return it->second; // Gets the cell value from the cell name key specified earlier
}

/*
* Adds a client useing their id to client_list
*/
void spreadsheet::add_client(int id)
{
	client_list.push_back(id);
}

/*
* removes client from list, 
* changes client_list's index to -1 if found
*/
void spreadsheet::remove_client(int id)
{
	for (int i = 0; i < client_list.size(); i++)
	{
		//if id is found then change to -1
		if (id == client_list[i])
			client_list[i] = -1;
	}
}

/*
* returns the list of clients currently connected
* to this spreadsheet
*/
std::vector<int> spreadsheet::give_client()
{
	std::vector<int> active_clients;
	

	for (int i = 0; i < client_list.size(); i++)
	{
		if (client_list[i] != -1)
		{
			active_clients.push_back(client_list[i]);
		}
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
