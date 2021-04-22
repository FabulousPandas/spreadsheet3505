/*
head comment


*/

#include "handle_connection.h"


typedef boost::shared_ptr<handle_connection> pointer;

handle_connection::handle_connection(boost::asio::io_context& io_context)
	: socket_(io_context)
{

}

pointer handle_connection::create(boost::asio::io_context& io_context)
{
	return pointer(new handle_connection(io_context));
}

boost::asio::ip::tcp::socket& handle_connection::socket()
{
	return socket_;
}

void handle_connection::start()
{

}



