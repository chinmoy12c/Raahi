package com.xsparks.Raahi.dao;

import com.xsparks.Raahi.models.POI;

import org.springframework.data.jpa.repository.JpaRepository;

public interface POIRepo extends JpaRepository<POI, Integer> {
    
}
