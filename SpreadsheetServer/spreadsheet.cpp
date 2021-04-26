/*
 Spreadsheet class. This spreadsheet object can holds the cells 
 of the spreadsheet and the connects (clients)
 Written by Malik Qader 4/25/2021

*/


#include "spreadsheet.h"

spreadsheet::spreadsheet(std::string name)
{
	spreadsheet_name = name;
}


void spreadsheet::add_to_q(std::string message)
{
	message_q.push_back(message);
}


bool spreadsheet::save_spreadsheet()
{
	return false;
}

void proccess_message(std::string)
{

}

void add_client(int id)
{
	client_list.push_back(id);
}

/*
* removes client from list
*/
void remove_client(int id)
{
	for (int i = 0; i < client_list.size(); i++)
	{
		//if id is found then change to -1
		if (id == client_list[i])
			client_list[i] = -1;
	}
}

std::vector<int> client_list()
{
	return give_clients;
}
