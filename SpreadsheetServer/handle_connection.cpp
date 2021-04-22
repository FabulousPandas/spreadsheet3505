/*
Class that deals with handling a connection between a client and a server
Written by Malik Qader and Dylan Hansen
*/

#include "handle_connection.h"

// Set an alias for the shared pointer to be "pointer"
typedef boost::shared_ptr<handle_connection> pointer;

handle_connection::handle_connection(boost::asio::io_context& io_context)
	: socket_(io_context) // Creates a socket object with the appropriate io_context passed to it
{

}

/*
 * Function which creates a smart pointer that points to this class
 */ 
pointer handle_connection::create(boost::asio::io_context& io_context)
{
	return pointer(new handle_connection(io_context));
}

/*
 * Getter function for the socket object that represents this connection
 */
boost::asio::ip::tcp::socket& handle_connection::socket()
{
	return socket_;
}

/*
 * The first steps of communication between the server and client
 */
void handle_connection::start()
{
	socket_.async_read_some(
		boost::asio::buffer(delivered_message, max_length),
		boost::bind(&handle_connection::read_handler,
			shared_from_this(),
			boost::asio::placeholders::error,
			boost::asio::placeholders::bytes_transferred));
}

void handle_connection::read_handler(const boost::system::error_code& err, size_t bytes_transferred)
{
}



