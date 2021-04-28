/*
* Header file for the class which listens for a new client to connect
* Written by Malik Qader and Dylan Hansen
*/

#ifndef LISTENER_H
#define LISTENER_H

#include <boost/asio.hpp>
#include <vector>
#include <string>
#include <boost/enable_shared_from_this.hpp>
#include <boost/bind.hpp>
#include "handle_connection.h"
#include "server.h"


class listener
{
public:


	listener(boost::asio::io_context& io_context, server* server);


private:


	boost::asio::ip::tcp::acceptor acceptor_obj;

	boost::asio::io_context& io_context_obj;

	server* the_server;


	void start_accept();

	void handle_accept(handle_connection::pointer new_connection,
		const boost::system::error_code& error);



};

#endif
