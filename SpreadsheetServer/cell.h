/*

*/

#ifndef CELL_H
#define CELL_H

#include <string>
#include <stack>


class cell
{
public:

	cell(std::string cell_name);

	void add_edit(std::string edit);

	std::string redo_cell();

	std::string undo_cell();

	std::string cell_content();


private:

	//cell name
	std::string cell_name;
	//stack of edits
	std::stack undo_stack;
	//stack of undo edits
	std::stack redo_stack;


};








#endif 
