package com.xsparks.Raahi.models;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.OneToOne;

import org.springframework.lang.NonNull;

@Entity
public class POI {
    @Id
    @GeneratedValue
    @NonNull
    int id;

    @NonNull
    @OneToOne
    PathNode pathNode;

    @NonNull
    String poiName;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public PathNode getNode() {
        return pathNode;
    }

    public void setNode(PathNode pathNode) {
        this.pathNode = pathNode;
    }

    public String getPoiName() {
        return poiName;
    }

    public void setPoiName(String poiName) {
        this.poiName = poiName;
    }
}
