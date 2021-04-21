/*
Header comment
*/
#include "server.h"
#include "listener.h"
#include "handle_connection.h"


using namespace boost::asio;



	listener::listener(io_context& io_context)
		: io_context_obj(io_context),
		acceptor_obj(io_context, ip::tcp::endpoint(ip::tcp::v4(), 1100))
	{
		start_accept();
	}

	void listener::start_accept()
	{

		handle_connection::pointer new_connection =
			handle_connection::create(io_context_obj);

		handle_connection* handler = new handle_connection(io_context_obj);
		acceptor_obj.async_accept(new_connection->socket(),
		        boost::bind(&handle_connection::handle_accept, handler, new_connection,
				boost::asio::placeholders::error));
	}



