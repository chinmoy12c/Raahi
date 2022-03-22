package com.xsparks.Raahi.models;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.ManyToOne;

import org.springframework.lang.NonNull;

@Entity
public class Edge {
    @Id
    @GeneratedValue
    @NonNull
    int id;

    @NonNull
    @ManyToOne
    PathNode from;

    @NonNull
    @ManyToOne
    PathNode to;

    @NonNull
    double distance;

    public int getId() {
        return id;
    }
    
    public void setId(int id) {
        this.id = id;
    }

    public PathNode getFrom() {
        return from;
    }

    public void setFrom(PathNode from) {
        this.from = from;
    }

    public PathNode getTo() {
        return to;
    }

    public void setTo(PathNode to) {
        this.to = to;
    }

    public double getDistance() {
        return distance;
    }

    public void setDistance(double distance) {
        this.distance = distance;
    }
}
