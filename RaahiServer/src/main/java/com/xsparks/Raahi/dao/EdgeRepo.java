package com.xsparks.Raahi.dao;

import com.xsparks.Raahi.models.Edge;
import com.xsparks.Raahi.models.PathNode;

import org.springframework.data.jpa.repository.JpaRepository;

public interface EdgeRepo extends JpaRepository<Edge, Integer> {
    public Edge[] getByToIn(PathNode[] to);
}
