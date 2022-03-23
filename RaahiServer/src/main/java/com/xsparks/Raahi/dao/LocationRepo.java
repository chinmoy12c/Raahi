package com.xsparks.Raahi.dao;

import com.xsparks.Raahi.models.Location;

import org.springframework.data.jpa.repository.JpaRepository;

public interface LocationRepo extends JpaRepository<Location, Integer> {
    public Location getByLocationName(String locationName);
}
