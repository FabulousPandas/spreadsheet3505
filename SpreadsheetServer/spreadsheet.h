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
	spreadsheet(std::string name);

	void add_to_q(std::string message);

	bool save_spreadsheet();

	std::string proccess_message(std::string);

	void add_client(int id);

	void remove_client(int id);

	std::vector<int> give_client();



private:

	std::queue<std::string> message_q;

	std::map<std::string, cell> cell_map;

	std::string spreadsheet_name;

	bool is_dependent(std::string);

	std::vector<int> client_list;




};


#endif 
