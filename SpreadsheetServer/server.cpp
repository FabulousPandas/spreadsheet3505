#include "server.h"
#include <iostream>
#include <iterator>
#include <algorithm>
#include <string>
#include "listener.h"

using namespace boost::asio;

int main()
{

    try
    {

        io_context io_context;
        listener listen(io_context);
        io_context.run();

    }
    catch (std::exception& e)
    {
        std::cerr << e.what() << std::endl;
    }

    return 0;
}




