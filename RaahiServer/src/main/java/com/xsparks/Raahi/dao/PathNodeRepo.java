package com.xsparks.Raahi.dao;

import com.xsparks.Raahi.models.Location;
import com.xsparks.Raahi.models.PathNode;

import org.springframework.data.jpa.repository.JpaRepository;

public interface PathNodeRepo extends JpaRepository<PathNode, Integer>{
    public PathNode[] getByLocation(Location location);
}
