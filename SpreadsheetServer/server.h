/*
Server class header

*/
#ifndef SERVER_H
#define SERVER_H

#include <boost/asio.hpp>
#include <boost/filesystem.hpp>
#include <boost/thread/thread.hpp>
#include <vector>
#include <map>
#include <string>
#include "spreadsheet.h"
#include <algorithm>


class server
{

	struct Alphabetical {
		
		/*
 		* Compares 2 strings in complete alphabetical order (case independent)
 		*/
		bool operator() (const std::string& a, const std::string& b) const
		{
			std::string str1(a);
			std::string str2(b);
			std::transform(str1.begin(), str1.end(), str1.begin(), [](unsigned char c){ return std::tolower(c); }); // Converts a to all lowercase	
			std::transform(str2.begin(), str2.end(), str2.begin(), [](unsigned char c){ return std::tolower(c); }); // Converts b to all lowercase
			return a < b;
		}

	};

public:

	server();
	std::string get_list_of_spreadsheets();
	std::map<std::string, spreadsheet*, Alphabetical> spreadsheets;
	spreadsheet* open_sheet(std::string filename);
	int get_ID();
	void polling();

private:


	std::string directory = "SavedSpreadsheets";
	std::vector<int> clients;
	int curID = 0;

	void put_spreadsheets_in_map();

	void process_message(spreadsheet* cur_sheet);

};





#endif
