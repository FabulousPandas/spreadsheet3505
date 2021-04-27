/*
handle connection class header

*/
#ifndef HANDLE_CONNECTION_H
#define	HANDLE_CONNECTION_H


#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>
#include <string>
#include <boost/bind.hpp>
#include "server.h"


class handle_connection
	:public boost::enable_shared_from_this<handle_connection>
{
public:

	handle_connection(boost::asio::io_context& io_context);

	typedef boost::shared_ptr<handle_connection> pointer;

	static pointer create(boost::asio::io_context& io_context);

	boost::asio::ip::tcp::socket& socket();

	void start(server serv);




private:

        boost::asio::ip::tcp::socket socket_;

	server the_server;
  
	enum { max_length = 512 };
	char delivered_message[max_length];
	std::string message_buffer;
	int con_state;
	std::string client_username;
	int ID;



	void read_handler(const boost::system::error_code& err,
		size_t bytes_transferred);

	void write_handler(const boost::system::error_code& err, size_t bytes_transferred);

	void read_message();

	void send_message(std::string message);

	bool complete_handshake_message();


};


#endif 
