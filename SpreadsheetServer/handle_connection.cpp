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
	client_username = "";
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
void handle_connection::start(server serv)
{
	// Gives this class the server object
	the_server = serv;
	// Sets the state of the connection to the part where it receives the username
	con_state = 1;
	
	read_message(); // Listens for a client message and calls the read_handler once on has been made
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
					client_username = message_buffer;
					std::cout << "USERNAME IS " << client_username << std::endl; //TODO: REMOVE (FOR TESTING ONLY)
					std::string spreadsheet_list = the_server.get_list_of_spreadsheets(); // Gets a list of spreadsheets from the server
					send_message(spreadsheet_list);
				}
				message_buffer = "";
				break;
			// Getting filename part of handshake
			case 2:
				if (complete_handshake_message())
				{
					con_state = 0;
					std::cout << "FILENAME IS " << message_buffer << std::endl; //TODO: REMOVE (FOR TESTING ONLY)
					this_sheet = the_server.open_sheet(message_buffer);
					ID = the_server.get_ID();
					send_message(std::to_string(ID) + '\n'); // TODO: ADD CREATING OR GETTING CELL DATA FROM FILE CHOSEN
					con_state = 2;
				}
				message_buffer = "";
				break;
			// Editing the spreadsheet communication
			case 3:
				read_message();
				break;
			// Shutting down the connection
			case 4:
				break;
		}
	}
}

/*
 * The function that deals with what to do once a message has been sent to the client
 */
void handle_connection::write_handler(const boost::system::error_code& err, size_t bytes_transferred)
{
	if(!err)
	{
		switch(con_state)
		{
			// Right after the server has sent the client the list of spreadsheets
			case 1:
				con_state = 2;
				read_message();
				break;
			case 2:
				con_state = 3;
				read_message();
				break;
			case 3:
				read_message();
				break;
			case 4:
				break;
		}
	}
}

void handle_connection::read_message()
{
	socket_.async_read_some(
		boost::asio::buffer(delivered_message, max_length), // Puts the message sent to the server in the delivered_message buffer
		boost::bind(&handle_connection::read_handler, // Calls read_handler once a message has been reveived by the server
			shared_from_this(),
			boost::asio::placeholders::error,
			boost::asio::placeholders::bytes_transferred));
}

void handle_connection::send_message(std::string message)
{
	socket_.async_write_some(
		boost::asio::buffer(message, max_length), // Sends the provided message to the client
       	 	boost::bind(&handle_connection::write_handler, // Calls write_handler once the message has successfully been sent
	                  shared_from_this(),
           		  boost::asio::placeholders::error,
	                  boost::asio::placeholders::bytes_transferred));
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

/*
* Takes in the entire message from client, splits message into
* a vector of strings containing:
* index 0 = requestType
* index 1 = cellName/ or empty
* index 2 = cellContents/ or empty
*/
std::vector<std::string> split_message(std::string message)
{
	rapidjson::Document doc;
	//doc.Parse(message);

	std::vector<std::string> dummy;
	return dummy;

}


