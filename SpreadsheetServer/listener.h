/*
* Listener Header
*
*/

#ifndef LISTENER_H
#define LISTENER_H

#include <boost/asio.hpp>
#include <vector>
#include <string>
#include <boost/enable_shared_from_this.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/bind.hpp>



class listener
{
public:


	listener(boost::asio::io_context& io_context);

private:

	boost::asio::ip::tcp::acceptor acceptor_obj;

	boost::asio::io_context& io_context_obj;


	void start_accept();



};

#endif