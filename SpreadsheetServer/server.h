/*
Server class header

*/
#ifndef SERVER_H
#define SERVER_H

#include <boost/asio.hpp>
#include <vector>
#include <string>





class server
{

public:


	std::string get_list_of_spreadsheets();

private:

	void polling();




};





#endif
