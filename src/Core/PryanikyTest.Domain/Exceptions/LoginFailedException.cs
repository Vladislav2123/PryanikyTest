﻿namespace PryanikyTest.Domain.Exceptions;

public class LoginFailedException : Exception
{
	public LoginFailedException() 
		: base("Wrong email or password") { }
    }