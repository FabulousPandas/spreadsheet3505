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

	void proccess_next_message();

	bool needs_to_proccess_message();

	void add_client(handle_connection* client);

	void add_client_id(int id, handle_connection* client);

	void remove_client(handle_connection* client);

	std::vector<handle_connection*> give_client();

	void disconnect_client(int id, handle_connection* client);

	void server_shutdown(std::string message);



private:
	std::queue<std::vector<std::string>> message_q;

	std::map<std::string, cell*> cell_map;

	std::vector<std::vector<std::string>> change_history;

	std::string spreadsheet_name;

	cell* get_cell(std::string cell_name);

	std::vector<handle_connection*> client_list;

	std::map<int, handle_connection*> id_to_client;

	bool is_cyclic_dependency();

	void build_from_file();

	void remake_file_from_history();

	void send_client_error(std::string client_id, std::string cell_name, std::string error_message);
	
	void visit(std::string start, std::string cur_cellname, std::vector<std::string>& visited, bool& cyclic_dependency);




};


#endif 
