/*
handle connection class header


*/
#ifndef HANDLE_CONNECTION_H
#define	HANDLE_CONNECTION_H


#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>
#include <string>
#include <boost/bind.hpp>



class handle_connection
	:public boost::enable_shared_from_this<handle_connection>
{
public:

	handle_connection(boost::asio::io_context& io_context);

	typedef boost::shared_ptr<handle_connection> pointer;

	static pointer create(boost::asio::io_context& io_context);

	boost::asio::ip::tcp::socket& socket();

	void start();




private:

        boost::asio::ip::tcp::socket socket_;
  
	enum { max_length = 512 };
	char delivered_message[max_length];
	std::string message_buffer;
	int con_state;




	void read_handler(const boost::system::error_code& err,
		size_t bytes_transferred);

	bool complete_handshake_message();


};


#endif 
