﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class FileSystemElement
{
	private object name_
	{
		get;
		set;
	}

	private object path_
	{
		get;
		set;
	}

	private object size_
	{
		get;
		set;
	}

	private object parent_elem_
	{
		get;
		set;
	}

	public virtual void Read()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetParentElem()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetName()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetSize()
	{
		throw new System.NotImplementedException();
	}

	public virtual void GetPath()
	{
		throw new System.NotImplementedException();
	}

	public virtual void CheckName()
	{
		throw new System.NotImplementedException();
	}

}

