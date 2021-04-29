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
void handle_connection::start(server* serv)
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
					std::string spreadsheet_list = the_server->get_list_of_spreadsheets(); // Gets a list of spreadsheets from the server
					send_message(spreadsheet_list);
					message_buffer = "";
				}
				break;
			// Getting filename part of handshake
			case 2:
				if (complete_handshake_message())
				{
					con_state = 0;
					std::cout << "FILENAME IS " << message_buffer << std::endl; //TODO: REMOVE (FOR TESTING ONLY)
					this_sheet = the_server->open_sheet(message_buffer);
					this_sheet->add_client(this);
					message_buffer = "";
				}
				break;
			// Editing the spreadsheet communication
			case 3:
				if (complete_json_message())
				{
					std::string json_message = message_buffer;
					std::vector<std::string> message = split_message(json_message);
					if (message.at(0) == "selectCell")
					{
						message.push_back(std::to_string(ID));
						message.push_back(client_username);
					}
					this_sheet->add_to_q(message);
					std::cout << "Message received: " << message_buffer << std::endl;
					message_buffer = "";
				}
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
				ID = the_server->get_ID();
				send_message(std::to_string(ID) + '\n'); // TODO: ADD CREATING OR GETTING CELL DATA FROM FILE CHOSEN
				con_state = 3;
				read_message();
				break;
			case 3:
				//read_message();
				break;
			case 4:
				break;
		}
	}
}
	
void handle_connection::server_response(std::vector<std::string> message)
{
	rapidjson::Document d;
	d.SetObject();

	rapidjson::Document::AllocatorType& allocator = d.GetAllocator();

	rapidjson::Value val(message.at(0).c_str(), allocator);
	d.AddMember("messageType", val, allocator);
	
	if (message.at(0) == "cellUpdated")
	{
		rapidjson::Value val1(message.at(1).c_str(), allocator);
		d.AddMember("cellName", val1, allocator);
		rapidjson::Value val2(message.at(2).c_str(), allocator);
		d.AddMember("contents", val2, allocator);
	}
	else if (message.at(0) == "cellSelected")
	{
		rapidjson::Value val1(message.at(1).c_str(), allocator);
		d.AddMember("cellName", val1, allocator);
		//rapidjson::Value val2(std::stoi(message.at(2)), allocator);
		d.AddMember("selector", std::stoi(message.at(2)), allocator);
		rapidjson::Value val3(message.at(3).c_str(), allocator);
		d.AddMember("selectorName", val3, allocator);
	}
	else if (message.at(0) == "disconnected")
	{
		rapidjson::Value val1(message.at(1).c_str(), allocator);
		d.AddMember("user", val1, allocator);
	}
	else if (message.at(0) == "requestError")
	{
		rapidjson::Value val1(message.at(1).c_str(), allocator);
		d.AddMember("cellName", val1, allocator);
		rapidjson::Value val2(message.at(2).c_str(), allocator);
		d.AddMember("message", val2, allocator);
	}
	else if (message.at(0) == "serverError")
	{
		rapidjson::Value val1(message.at(1).c_str(), allocator);
		d.AddMember("message", val1, allocator);
	}
	

	rapidjson::StringBuffer str;
	rapidjson::Writer<rapidjson::StringBuffer> writer(str);
	d.Accept(writer);
	std::string output = str.GetString();

	std::cout << "SENDING TO CLIENT: " << output << std::endl;

	send_message(output + '\n');
	
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
 * Returns true if the message received by the client is complete by the rules of a handshake method.
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
 * Returns true if the message received by the client is a complete json message.
 * Also puts the message inside of the message buffer regardless of if it is complete or not
 */
bool handle_connection::complete_json_message()
{
	for (int i = 0; i < sizeof(delivered_message)/sizeof(*delivered_message); i++)
	{
		message_buffer += delivered_message[i];
		if (delivered_message[i] == '}')
		{
			rapidjson::Document doc;
			if (!doc.Parse(message_buffer.c_str()).HasParseError())
				return true;
		}
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
std::vector<std::string> handle_connection::split_message(std::string message)
{
	const char* json = message.c_str();

	rapidjson::Document doc;
	doc.Parse(json);

	std::vector<std::string> values;

	rapidjson::Value::ConstMemberIterator itr = doc.FindMember("requestType"); // Iterator that points at the member "requestType" in the json
	//if (itr == doc.MemberEnd())
		//TODO AAAA ERROR
	values.push_back(itr->value.GetString()); // Pushes the value of that member to the first slot of the vector
	if(values.at(0) != "undo") // If it's a requestType that has more members
	{
		rapidjson::Value::ConstMemberIterator itr = doc.FindMember("cellName");
		values.push_back(itr->value.GetString());
		if(values.at(0) == "editCell") // If it's a request type that has a 3rd member
		{
			rapidjson::Value::ConstMemberIterator itr = doc.FindMember("contents");
			values.push_back(itr->value.GetString());
		}

	}

	return values;

}


