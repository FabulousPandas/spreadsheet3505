/*
head comment


*/

#include "handle_connection.h"

class handle_connection
	: public boost::enable_shared_from_this<handle_connection>
{
	typedef boost::shared_ptr<handle_connection> pointer;

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

};