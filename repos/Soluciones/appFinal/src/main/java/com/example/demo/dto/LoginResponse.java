package com.example.demo.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class LoginResponse {
    private boolean ok;
    private String token;
    private UserInfo user;
    private String error;
    
    public LoginResponse(boolean ok, String token, UserInfo user) {
        this.ok = ok;
        this.token = token;
        this.user = user;
    }
    
    public LoginResponse(boolean ok, String error) {
        this.ok = ok;
        this.error = error;
    }
    
    @Data
    @NoArgsConstructor
    @AllArgsConstructor
    public static class UserInfo {
        private Long id;
        private String username;
    }
}
