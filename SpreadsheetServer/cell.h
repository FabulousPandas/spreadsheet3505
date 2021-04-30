/*
	This cell class contains the cells contents and edits.
	Will be used in the spreadsheet class.
	Written by Malik Qader and Dylan Hansen 4/29/2021
*/

#ifndef CELL_H
#define CELL_H

#include <string>
#include <stack>
#include <vector>


class cell
{
public:

	cell(std::string cell_name);

	void add_edit(std::string edit);

	void remove_edit();

	std::string undo_revert();

	std::string revert();

	std::string cell_content();

	std::vector<std::string> get_dependent_list();


private:

	//cell name
	std::string cell_name;
	//stack of edits
	std::stack<std::string> undo_stack;
	//stack of undo edits
	std::stack<std::string> redo_stack;

};








#endif 
