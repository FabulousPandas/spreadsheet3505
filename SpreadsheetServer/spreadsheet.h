/*

*/

#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <queue>
#include <string>
#include <map>
#include <vector>
#include "cell.h"


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

	void add_client(int id);

	void remove_client(int id);

	std::vector<int> give_client();



private:

	std::queue<std::vector<std::string>> message_q;

	std::map<std::string, cell> cell_map;

	std::vector<std::vector<std::string>> change_history;

	std::string spreadsheet_name;

	cell get_cell(std::string cell_name);

	bool is_dependent(std::vector<std::string> message);

	std::vector<int> client_list;




};


#endif 
