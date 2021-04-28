/*
A class which listens for client connections and redirects a newly made connection to the handle_connection class
Written by Dylan Hansen and Malik Qader
*/
#include "listener.h"

using namespace boost::asio;


	listener::listener(io_context& io_context, server* serv)
		: io_context_obj(io_context), // Clones io_context to another object which is accessible to the entire class
		acceptor_obj(io_context, ip::tcp::endpoint(ip::tcp::v4(), 1100))
	{
		the_server = serv;
		start_accept(); // Starts looking for client connections on port 1100
	}

	/*
	 * Function that is called to look for client connections on the port
	 */
	void listener::start_accept()
	{

		// Smart pointer to the handle_connection class
		handle_connection::pointer new_connection =
			handle_connection::create(io_context_obj);

		// Looks for client connections and calls handle_accept once a connection is made
		acceptor_obj.async_accept(new_connection->socket(),
		        boost::bind(&listener::handle_accept, this, new_connection,
				boost::asio::placeholders::error));
	}

	/*
	 * Handles what to do once a successful connection is made between a new client and the server
	 */
	void listener::handle_accept(handle_connection::pointer new_connection,
		const boost::system::error_code& error)
	{
		if(!error)
		{
			new_connection->start(the_server); // Starts the connection handler to begin communication between the new client and the server
		}
		start_accept(); // Creates and async loop of looking for new clients to connect
	
	}



