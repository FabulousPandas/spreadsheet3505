#include "cell.h"
	
	/*
	* Cell constructor
	*/
	cell::cell(std::string name)
	{
		cell_name = name;
		undo_stack.push("");
	}
	/*
	* changes cell contenst to param edit
	*/
	void cell::add_edit(std::string edit)
	{
		//add edit to stack
		undo_stack.push(edit);
		
	}
	/*
	* undoes the act of reverting
	* if the stack was empty then "empty"
	* is returned
	*/
	std::string cell::undo_revert()
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
	* removes last edit from the stack
	*/
	void cell::remove_edit()
	{
		undo_stack.pop();
	}
	/*
	* reverts cell to last change
	* or if the stack was empty then "empty"
	* is returned
	*/
	std::string cell::revert()
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
