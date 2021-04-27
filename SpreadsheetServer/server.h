/*
Server class header

*/
#ifndef SERVER_H
#define SERVER_H

#include <boost/asio.hpp>
#include <boost/filesystem.hpp>
#include <vector>
#include <string>





class server
{

public:


	std::string get_list_of_spreadsheets();

private:
	std::string directory = "SavedSpreadsheets";
	
	static bool alphabetical_compare(std::string a, std::string b);


	void polling();




};





#endif
