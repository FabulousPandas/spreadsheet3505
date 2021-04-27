#include "cell.h"
	
	/*
	* Cell constructor
	*/
	cell::cell(std::string name)
	{
		cell_name = name;

	}
	/*
	* changes cell contenst to param edit
	*/
	void cell::add_edit(std::string edit)
	{
		//add edit to stack
		undo_stack.push(edit);
		
		//clear redo stack
		for (int i = 0; i < redo_stack.size(); i++)
			redo_stack.pop();
	}
	/*
	* changes cell to most recent undo
	*/
	std::string cell::redo_cell()
	{
		if (redo_stack.empty())
			return "empty";

		std::string popped;
		//save popped edit
		popped = redo_stack.top();
		redo_stack.pop();
		//push popped edit onto redo stack
		undo_stack.push(popped);

		return popped;
	}
	/*
	* changes cell to most recent edit
	*/
	std::string cell::undo_cell()
	{
		if (undo_stack.empty())
			return "empty";

		std::string popped;
		//save popped edit
		popped = undo_stack.top();
		undo_stack.pop();
		//push popped edit onto redo stack
		redo_stack.push(popped);

		return popped;
	}

	/*
	* returns the contents of the cell
	*/
	std::string cell::cell_content()
	{
		return undo_stack.top();
	}
