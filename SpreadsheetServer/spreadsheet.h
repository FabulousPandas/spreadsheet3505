/*

*/

#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <queue>
#include <string>
#include <map>
#include <vector>



class spreadsheet 
{
public:

	spreadsheet(std::string name);

	std::queue<std::string> message_q;

	std::vector<int> clients;

	void add_to_q();

	bool save_spreadsheet();

	void proccess_message(std::string);



private:

	std::map<std::string cell_name, std::vector<std::string> edits> cells;




	bool is_dependent(std::string);




};


#endif 
