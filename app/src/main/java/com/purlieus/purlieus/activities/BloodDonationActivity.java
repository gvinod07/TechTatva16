package com.purlieus.purlieus.activities;

import android.os.Build;
import android.os.Bundle;
import android.os.PersistableBundle;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;

import com.purlieus.purlieus.adapters.BDPagerAdapter;
import com.purlieus.purlieus.R;
import com.purlieus.purlieus.fragments.DonateFragment;
import com.purlieus.purlieus.fragments.SeekFragment;

/**
 * Created by anurag on 3/10/16.
 */
public class BloodDonationActivity extends AppCompatActivity implements ViewPager.OnPageChangeListener{


    SeekFragment seekFrag;
    DonateFragment donFrag;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_blood_donation);

        setTheme(R.style.HealthTheme);

        Toolbar toolbar = (Toolbar)findViewById(R.id.bd_toolbar);
        setSupportActionBar(toolbar);
        if (getSupportActionBar()!=null) {
            getSupportActionBar().setTitle(R.string.blood_donation);
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
            getSupportActionBar().setElevation(0);
        }

        TabLayout tabLayout = (TabLayout)findViewById(R.id.bd_tab_layout);
        ViewPager viewPager = (ViewPager)findViewById(R.id.bd_view_pager);

        seekFrag = new SeekFragment();
        donFrag= new DonateFragment();

        BDPagerAdapter pagerAdapter = new BDPagerAdapter(getSupportFragmentManager());
        pagerAdapter.addFragment(seekFrag, "Seek");
        pagerAdapter.addFragment(donFrag, "Donate");
        viewPager.setAdapter(pagerAdapter);

        tabLayout.setupWithViewPager(viewPager);

    }

    @Override
    public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {

    }

    @Override
    public void onPageSelected(int position) {
        switch(position){
            case 0:{
                seekFrag.loadDonors();
                break;
            }
            case 1:{
                donFrag.loadSeekers();
                break;
            }

        }
    }

    @Override
    public void onPageScrollStateChanged(int state) {

    }
}
