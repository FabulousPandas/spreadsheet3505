/*

*/

#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <queue>
#include <string>
#include <map>
#include <vector>
#include "cell.h"

class handle_connection;

class spreadsheet 
{


public:

	spreadsheet();
	spreadsheet(std::string filepath);

	void add_to_q(std::vector<std::string> message);

	void build_from_file();

	void write_message_to_spreadsheet(std::vector<std::string> message);

	void remake_file_from_history();

	std::string proccess_next_message();

	bool needs_to_proccess_message();

	void add_client(handle_connection* client);

	void remove_client(handle_connection* client);

	std::vector<handle_connection*> give_client();



private:
	std::queue<std::vector<std::string>> message_q;

	std::map<std::string, cell> cell_map;

	std::vector<std::vector<std::string>> change_history;

	std::string spreadsheet_name;

	cell get_cell(std::string cell_name);

	bool is_dependent(std::vector<std::string> message);

	std::vector<handle_connection*> client_list;




};


#endif 
