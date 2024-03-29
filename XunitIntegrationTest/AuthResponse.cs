﻿using System;

namespace XunitIntegrationTest
{
    public class AuthResponse
    {
        public string accessToken { get; set; }

        public DateTime? expirationDateTime { get; set; }

        public DateTime? dateIssued { get; set; }

        public string refreshToken { get; set; }
    }
}
