/*
Server class header

*/
#ifndef SERVER_H
#define SERVER_H

#include <boost/asio.hpp>
#include <boost/filesystem.hpp>
#include <vector>
#include <string>
#include "spreadsheet.h"



class server
{

public:


	std::string get_list_of_spreadsheets();
	int get_ID();

private:
	std::string directory = "SavedSpreadsheets";
	std::vector<int> clients;
	std::vector<spreadsheet> spreadsheets;
	int curID = 0;
	
	static bool alphabetical_compare(std::string a, std::string b);


	void polling();




};





#endif
