package com.xsparks.Raahi.controller;

import java.util.HashMap;

import javax.servlet.http.HttpServletRequest;

import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class RouteController {
    @PostMapping("/saveRoute")
    void saveRoute(@RequestBody HashMap<String, Object> request) {
        
    }
}
