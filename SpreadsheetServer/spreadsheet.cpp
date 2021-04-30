/*
 Spreadsheet class. This spreadsheet object can holds the cells 
 of the spreadsheet and the connects (clients)
 Written by Malik Qader and Dylan Hansen 4/30/2021

*/


#include "spreadsheet.h"
#include <boost/filesystem.hpp>
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

/*
 * Builds a history of changes from the spreadsheet file.
 */
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
	// For every line in the file
	while (std::getline(file, str))
	{
		// If we have reached a complete message to add to the log
		if(str == "")
		{
			change_history.push_back(message);
			history_index++;
			message_index = 0;
		}
		else
		{
			// Adds the line to the message
			message.at(message_index) = str;
			// Adds this to a cell if needed
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
* Saves the spreadsheet in this format:
* Every line is a seperate element of a message
* Once a complete message has been written, write an additional new line
*/
void spreadsheet::remake_file_from_history()
{
	boost::filesystem::path sheet_dir(spreadsheet_name);
	boost::filesystem::ofstream file(sheet_dir);
	// for every message in the histoy log
	for (int i = 0; i < change_history.size(); i++)
	{
		//write that message to the file in the formate listed above
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

	if (message.at(0) == "editCell")
	{
		// gets cell object of the cell needed to edit
		cell* this_cell = get_cell(message.at(1));	
		this_cell->add_edit(message.at(2));
		if(is_cyclic_dependency())
		{
			send_client_error(message.at(3), message.at(1), "Edit would cause cyclic dependency");
			this_cell->remove_edit();
			return;
		}
		//Remove appended ID from the message
		message.pop_back();
		message.resize(3);
		//add message to history
		change_history.push_back(message);
		remake_file_from_history();
			
		//sends the message back to the client as a "cellUpdated" message
		message.at(0) = "cellUpdated";
		for (int i = 0; i < client_list.size(); i++)
		{
			handle_connection* client = client_list.at(i);
			client->server_response(message);
		}
	}
	else if (message.at(0) == "selectCell")
	{
		// Sends back the select cell message as a "cellSelected" message
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

		// Gets the previous message in the history
		std::vector<std::string> prev_message = change_history.back();
		cell* prev_cell = get_cell(prev_message.at(1));
		//If it was an edit request we send the client a request to change the cell's value to the previous value it held
		if (prev_message.at(0) == "editCell")
		{
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
		// Removes the most recent edit from the change history
		change_history.pop_back();
		remake_file_from_history();
	}

}
	
/*
 * Sends the client with the specified ID an error message
 */
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

void spreadsheet::disconnect_client(int id, handle_connection* client)
{
	id_to_client.erase(id);
	client_list.erase(std::find(client_list.begin(), client_list.end(), client));
	std::vector<std::string> message;
	message.push_back("disconnected");
	message.push_back(std::to_string(id));
	// Sends each client a disconnected message
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
	// Sends each client a server shutdown message
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
	bool cyclic_dependency = false;
	std::vector<std::string> visited;
	// Visits every cell and follows the cells that it uses for its value
	for (std::map<std::string, cell*>::iterator it = cell_map.begin(); it != cell_map.end(); it++)
	{
		std::string cur_cellname = it->first;
		if(std::find(visited.begin(), visited.end(), cur_cellname) == visited.end()) // if the current cell has not been visited yet
		{
			visit(cur_cellname, cur_cellname, visited, cyclic_dependency); 
		}
	}
	return cyclic_dependency;
}

/*
 * Visits a cell as part of checking for cyclic dependencies
 */
void spreadsheet::visit(std::string start, std::string cur_cellname, std::vector<std::string>& visited, bool& cyclic_dependency)
{
	visited.push_back(cur_cellname);
	cell* cur_cell = cell_map[cur_cellname];
	std::vector<std::string> dependent_list = cur_cell->get_dependent_list();
	for (int i = 0; i < dependent_list.size(); i++)
	{
		std::string dependent = dependent_list.at(i);
		// If a cell goes in a circle following this path then a cyclic dependency has occured
		if(dependent == start)
			cyclic_dependency = true;
		else if (std::find(visited.begin(), visited.end(), dependent) != visited.end())
			visit(start, dependent, visited, cyclic_dependency);
	}
}
