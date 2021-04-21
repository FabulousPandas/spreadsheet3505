/*
head comment


*/

#include "handle_connection.h"


	

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

