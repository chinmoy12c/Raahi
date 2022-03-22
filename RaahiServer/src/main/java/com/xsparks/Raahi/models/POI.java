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
    PathNode node;

    @NonNull
    String poiName;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public PathNode getNode() {
        return node;
    }

    public void setNode(PathNode node) {
        this.node = node;
    }

    public String getPoiName() {
        return poiName;
    }

    public void setPoiName(String poiName) {
        this.poiName = poiName;
    }
}
