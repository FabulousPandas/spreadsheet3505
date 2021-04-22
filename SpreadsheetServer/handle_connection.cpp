/*
Class that deals with handling a connection between a client and a server
Written by Malik Qader and Dylan Hansen
*/

#include "handle_connection.h"
#include <iostream> //TODO: REMOVE (FOR TESTING ONLY)

// Set an alias for the shared pointer to be "pointer"
typedef boost::shared_ptr<handle_connection> pointer;

handle_connection::handle_connection(boost::asio::io_context& io_context)
	: socket_(io_context) // Creates a socket object with the appropriate io_context passed to it
{
	message_buffer = "";
	con_state = 0;
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
	// Sets the state of the connection to the part where it receives the username
	con_state = 1;

	socket_.async_read_some(
		boost::asio::buffer(delivered_message, max_length), // Puts the message sent to the server in the delivered_message buffer
		boost::bind(&handle_connection::read_handler, // Calls read_handler once a message has been reveived by the server
			shared_from_this(),
			boost::asio::placeholders::error,
			boost::asio::placeholders::bytes_transferred));
}

/*
 * The function that deals with what to do once a message has been received by the server
 */
void handle_connection::read_handler(const boost::system::error_code& err, size_t bytes_transferred)
{
	if (!err)
	{
		switch(con_state)
		{
			// Getting username part of handshake
			case 1:
				if (complete_handshake_message())
				{
					std::cout << "USERNAME IS " << message_buffer << std::endl; //TODO: REMOVE (FOR TESTING ONLY)
				}
				message_buffer = "";
				break;
			// Getting filename part of handshake
			case 2:
				break;
			// Editing the spreadsheet communication
			case 3:
				break;
			// Shutting down the connection
			case 4:
				break;
		}
	}
}

/*
 * Returns true if the message received by the server is complete by the rules of a handshake method.
 * Also puts the message inside of the message buffer regardless of if it is complete or not
 */
bool handle_connection::complete_handshake_message()
{
	for (int i = 0; i < sizeof(delivered_message)/sizeof(*delivered_message); i++)
	{
		if (delivered_message[i] == '\n')
		{
			return true;
		}
		message_buffer += delivered_message[i];
	}
	return false;
}



