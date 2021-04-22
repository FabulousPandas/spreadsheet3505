/*

*/

#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include <queue>



class spreadsheet 
{
public:

	spreadsheet(std::string name);

	std::queue message_q;

	std::vector<int> clients;

	void add_to_q();

	bool save_spreadsheet();



private:


};


#endif 