/*
handle connection class header


*/
#ifndef HANDLE_CONNECTION_H
#define	HANDLE_CONNECTION_H


#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>
#include <boost/shared_ptr.hpp>
#include <string>






class handle_connection
	:public boost::enable_shared_from_this<handle_connection>
{
public:

	handle_connection(boost::asio::io_context& io_context);

	typedef boost::shared_ptr<handle_connection> pointer;

	static pointer create(boost::asio::io_context& io_context);

	boost::asio::ip::tcp::socket& socket();

	void start();

	void handle_accept(pointer new_connection,
		const boost::system::error_code& error);



private:

	boost::asio::ip::tcp::socket socket_;

	std::string message_;




	void handle_write(const boost::system::error_code& /*error*/,
		size_t /*bytes_transferred*/);




};


#endif 