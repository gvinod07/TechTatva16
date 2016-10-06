package com.purlieus.purlieus.adapters;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by anurag on 6/10/16.
 */
public class ProfilePagerAdapter extends FragmentPagerAdapter {

    List<Fragment> fragmentsList = new ArrayList<>();
    List<Long> idList = new ArrayList<>();

    public ProfilePagerAdapter(FragmentManager fm) {
        super(fm);
    }

    public void addFragment(Fragment fragment, long id){
        fragmentsList.add(fragment);
        idList.add(id);
    }

    @Override
    public Fragment getItem(int position) {
        return fragmentsList.get(position);
    }

    @Override
    public int getCount() {
        return fragmentsList.size();
    }

    @Override
    public long getItemId(int position) {
        return idList.get(position);
    }
}
