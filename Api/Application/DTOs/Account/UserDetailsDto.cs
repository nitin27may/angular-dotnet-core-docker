﻿using System;

namespace Application.DTOs.Account
{
    public class UserDetailsDto
    {
        public string Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        public bool EmailConfirmed { get; set; }

        public string? PhoneNumber { get; set; }
    }
}