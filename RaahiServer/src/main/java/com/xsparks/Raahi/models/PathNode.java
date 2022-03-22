package com.xsparks.Raahi.models;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.ManyToOne;

import org.springframework.lang.NonNull;

@Entity
public class PathNode {
    @Id
    @GeneratedValue
    @NonNull
    int id;

    @NonNull
    String vector;

    @NonNull
    @ManyToOne
    Location location;

    public int getId() {
        return id;
    }
    
    public void setId(int id) {
        this.id = id;
    }

    public Location getLocation() {
        return location;
    }

    public void setLocation(Location location) {
        this.location = location;
    }

    public String getVector() {
        return vector;
    }

    public void setVector(String vector) {
        this.vector = vector;
    }
}
