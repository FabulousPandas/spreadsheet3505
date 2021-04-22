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



private:

	void polling();



	std::vector<std::string> client_list = std::vector<std::string>();

	std::vector<std::string> connection_list = std::vector<std::string>();

};





#endif