/*
head comment


*/

#include "handle_connection.h"


typedef boost::shared_ptr<handle_connection> pointer;

handle_connection(boost::asio::io_context& io_context)
	:socket(io_context)
{

}

static pointer create(boost::asio::io_context& io_context)
{
	return pointer(new handle_connection(io_context));
}

boost::asio::ip::tcp::socket& socket()
{
	return socket_;
}


void handle_accept(pointer new_connection,
	const boost::system::error_code& error)
{

}

