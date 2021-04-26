/*
 * The main class for dealing with the server code
 */ 
#include "server.h"
#include <iostream>
#include <iterator>
#include <algorithm>
#include <string>
#include "listener.h"

using namespace boost::asio;

std::vector<std::string> client_list;

std::vector<std::string> connection_list;

int main()
{

    try
    {

	server the_server;
        io_context io_context;
        listener listen(io_context, the_server);
        io_context.run();

    }
    catch (std::exception& e)
    {
        std::cerr << e.what() << std::endl;
    }


    return 0;
}

std::string server::get_list_of_spreadsheets()
{
	return "ss1.txt\nmalik\ndylan.png\n\n"; // TODO: REPLACE WITH ACTUAL FILE READING
}

    void server::polling()
    {
        bool idle = false;
        bool loop = true;


        while (loop)
        {
            for (int i = 0; i< client_list.size(); i++)
            {

            }
        }
    }








