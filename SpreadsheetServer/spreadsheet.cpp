/*
 Spreadsheet class. This spreadsheet object can holds the cells 
 of the spreadsheet and the connects (clients)
 Written by Malik Qader 4/25/2021

*/


#include "spreadsheet.h"

spreadsheet::spreadsheet()
{
	spreadsheet_name = "";
}

/*
* Spreadsheet constructor
*/
spreadsheet::spreadsheet(std::string name)
{
	spreadsheet_name = name;
}

/*
* adds a message from the client to the mesasge_q
*/
void spreadsheet::add_to_q(std::string message)
{
	message_q.push(message);
}

/*
* Saves the spreadsheet
*/
bool spreadsheet::save_spreadsheet()
{
	return false;
}

/*
* Implements one message from the message_q
* return "pass" if it was succesful 
* otherwise will return a string describing
* the error
*/
std::string spreadsheet::proccess_message(std::string message)
{
	if (!is_dependent(message))
	{

	}


	return "pass";
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
bool spreadsheet::is_dependent(std::string message)
{

	//TODO

	return false;
}
