package com.xsparks.Raahi.dao;

import com.xsparks.Raahi.models.POI;
import com.xsparks.Raahi.models.PathNode;

import org.springframework.data.jpa.repository.JpaRepository;

public interface POIRepo extends JpaRepository<POI, Integer> {
    public POI[] getByPathNodeIn(PathNode[] node);
}
